# Never Gonna

A Rust program that plays fragments of "Never Gonna Give You Up" in a Markov chain fashion, outputting raw PCM audio to stdout.

Based on the Markov chain transitions and audio resources from [Guowenxuan1031/NeverGonnaUnity](https://github.com/Guowenxuan1031/NeverGonnaUnity).

## Build

```
nix build
```

## Usage

```
./result/bin/never-gonna | aplay -f cd -t raw
```

The program runs indefinitely, outputting 16-bit stereo 44100 Hz PCM.

### AudioSocket mode

With `--audiosocket`, output is encoded as [AudioSocket](https://docs.asterisk.org/Configuration/Channel-Drivers/AudioSocket/) frames (type `0x15`: signed linear 16-bit 44100 Hz mono PCM, little-endian) split into ~20ms chunks. Stdin is set to nonblocking mode and drained after each frame; the program exits cleanly when stdin closes.

```
./result/bin/never-gonna --audiosocket | ...
```

## Markov Chain

The chain always starts at **NEVER → GONNA**, then branches randomly:

- **GONNA** → GIVE / LET / RUN / MAKE / SAY / TELL (each 1/6)
- **GIVE / MAKE** → YOU → CRY or UP (1/2 each)
- **LET** → YOU2 → DOWN → NEVER
- **RUN** → AROUND → AND → DESERT or HURT (1/2 each)
- **DESERT / HURT** → YOU1 → NEVER
- **SAY** → GOODBYE → NEVER
- **TELL** → ALIE → AND → ...

Every path eventually loops back to NEVER.
