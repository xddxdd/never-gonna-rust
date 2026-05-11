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
