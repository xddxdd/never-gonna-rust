use rand::Rng;
use std::io::{self, Read, Write};
use std::os::raw::c_int;
use std::os::unix::io::AsRawFd;

const NEVER: &[u8] = include_bytes!("../music/never.wav");
const GONNA: &[u8] = include_bytes!("../music/gonna.wav");
const GIVE: &[u8] = include_bytes!("../music/give.wav");
const LET: &[u8] = include_bytes!("../music/let.wav");
const YOU1: &[u8] = include_bytes!("../music/you1.wav");
const YOU2: &[u8] = include_bytes!("../music/you2.wav");
const YOU3: &[u8] = include_bytes!("../music/you3.wav");
const UP: &[u8] = include_bytes!("../music/up.wav");
const DOWN: &[u8] = include_bytes!("../music/down.wav");
const RUN: &[u8] = include_bytes!("../music/run.wav");
const AROUND: &[u8] = include_bytes!("../music/around.wav");
const AND: &[u8] = include_bytes!("../music/and.wav");
const DESERT: &[u8] = include_bytes!("../music/desert.wav");
const MAKE: &[u8] = include_bytes!("../music/make.wav");
const CRY: &[u8] = include_bytes!("../music/cry.wav");
const SAY: &[u8] = include_bytes!("../music/say.wav");
const GOODBYE: &[u8] = include_bytes!("../music/goodbye.wav");
const TELL: &[u8] = include_bytes!("../music/tell.wav");
const ALIE: &[u8] = include_bytes!("../music/alie.wav");
const HURT: &[u8] = include_bytes!("../music/hurt.wav");

#[derive(Clone, Copy, PartialEq)]
enum State {
    Never,
    Gonna,
    Give,
    Let,
    You,
    You1,
    You2,
    Up,
    Down,
    Run,
    Around,
    And,
    Desert,
    Make,
    Cry,
    Say,
    Goodbye,
    Tell,
    Alie,
    Hurt,
}

impl State {
    fn wav_data(&self) -> &[u8] {
        match self {
            State::Never => NEVER,
            State::Gonna => GONNA,
            State::Give => GIVE,
            State::Let => LET,
            State::You => YOU1,
            State::You1 => YOU2,
            State::You2 => YOU3,
            State::Up => UP,
            State::Down => DOWN,
            State::Run => RUN,
            State::Around => AROUND,
            State::And => AND,
            State::Desert => DESERT,
            State::Make => MAKE,
            State::Cry => CRY,
            State::Say => SAY,
            State::Goodbye => GOODBYE,
            State::Tell => TELL,
            State::Alie => ALIE,
            State::Hurt => HURT,
        }
    }
}

fn extract_pcm_data(wav_bytes: &[u8]) -> Vec<u8> {
    let data_str = b"data";
    let mut pos = 12;
    while pos + 8 <= wav_bytes.len() {
        let chunk_id = &wav_bytes[pos..pos + 4];
        let chunk_size = u32::from_le_bytes([
            wav_bytes[pos + 4],
            wav_bytes[pos + 5],
            wav_bytes[pos + 6],
            wav_bytes[pos + 7],
        ]);
        if chunk_id == data_str {
            let start = pos + 8;
            let end = start + chunk_size as usize;
            return wav_bytes[start..end].to_vec();
        }
        pos += 8 + chunk_size as usize;
        if pos % 2 != 0 {
            pos += 1;
        }
    }
    panic!("No data chunk found in WAV file");
}

fn stereo_to_mono(stereo: &[u8]) -> Vec<u8> {
    let sample_count = stereo.len() / 4;
    let mut mono = Vec::with_capacity(sample_count * 2);
    for i in 0..sample_count {
        let off = i * 4;
        let left = i16::from_le_bytes([stereo[off], stereo[off + 1]]);
        let right = i16::from_le_bytes([stereo[off + 2], stereo[off + 3]]);
        let mixed = ((left as i32 + right as i32) / 2) as i16;
        mono.extend_from_slice(&mixed.to_le_bytes());
    }
    mono
}

fn set_stdin_nonblocking() {
    extern "C" {
        fn fcntl(fd: c_int, cmd: c_int, arg: c_int) -> c_int;
    }
    const F_GETFL: c_int = 3;
    const F_SETFL: c_int = 4;
    const O_NONBLOCK: c_int = 2048;

    unsafe {
        let fd = io::stdin().as_raw_fd();
        let flags = fcntl(fd, F_GETFL, 0);
        fcntl(fd, F_SETFL, flags | O_NONBLOCK);
    }
}

fn drain_stdin() -> bool {
    let mut buf = [0u8; 1024];
    let mut stdin = io::stdin().lock();
    loop {
        match stdin.read(&mut buf) {
            Ok(0) => return true,
            Err(e) if e.kind() == io::ErrorKind::WouldBlock => return true,
            Err(_) => return false,
            Ok(_) => continue,
        }
    }
}

fn main() {
    let audiosocket = std::env::args().any(|a| a == "--audiosocket");

    let stdout = io::stdout();
    let mut out = io::BufWriter::new(stdout.lock());

    let mut rng = rand::thread_rng();
    let mut state = State::Never;

    if audiosocket {
        set_stdin_nonblocking();
    }

    loop {
        let pcm = extract_pcm_data(state.wav_data());

        if audiosocket {
            let mono = stereo_to_mono(&pcm);
            for chunk in mono.chunks(1764) {
                let len = chunk.len() as u16;
                let mut frame = Vec::with_capacity(3 + chunk.len());
                frame.push(0x15);
                frame.extend_from_slice(&len.to_be_bytes());
                frame.extend_from_slice(chunk);
                out.write_all(&frame).unwrap();
                out.flush().unwrap();

                if !drain_stdin() {
                    return;
                }
            }
        } else {
            out.write_all(&pcm).unwrap();
            out.flush().unwrap();
        }

        state = match state {
            State::Never => State::Gonna,
            State::Gonna => match rng.gen_range(0..6) {
                0 => State::Say,
                1 => State::Run,
                2 => State::Tell,
                3 => State::Make,
                4 => State::Give,
                _ => State::Let,
            },
            State::Give | State::Make => State::You,
            State::You => match rng.gen_range(0..2) {
                0 => State::Cry,
                _ => State::Up,
            },
            State::Let => State::You2,
            State::You2 => State::Down,
            State::Run => State::Around,
            State::Around => State::And,
            State::And => match rng.gen_range(0..2) {
                0 => State::Desert,
                _ => State::Hurt,
            },
            State::Desert | State::Hurt => State::You1,
            State::Say => State::Goodbye,
            State::Tell => State::Alie,
            State::Alie => State::And,
            State::Goodbye => State::Never,
            State::Down => State::Never,
            State::Cry => State::Never,
            State::Up => State::Never,
            State::You1 => State::Never,
        };
    }
}
