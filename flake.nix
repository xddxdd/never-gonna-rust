{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
    rust-overlay.url = "github:oxalica/rust-overlay";
    rust-overlay.inputs.nixpkgs.follows = "nixpkgs";
  };

  outputs = inputs @ { self, nixpkgs, flake-parts, rust-overlay }:
    flake-parts.lib.mkFlake { inherit inputs; } {
      systems = [
        "x86_64-linux"
        "aarch64-linux"
      ];

      imports = [
        flake-parts.flakeModules.easyOverlay
      ];

      perSystem = { config, self', pkgs, system, ... }:
        let
          pkgs = import nixpkgs {
            inherit system;
            overlays = [ (import rust-overlay) ];
          };
          rustToolchain = pkgs.rust-bin.stable.latest.default;
        in
        {
          packages.never-gonna = pkgs.rustPlatform.buildRustPackage {
            pname = "never-gonna";
            version = "0.1.0";
            src = ./.;
            cargoLock.lockFile = ./Cargo.lock;

            meta = {
              description = "Rust program that plays fragments of \"Never Gonna Give You Up\" in a Markov chain fashion";
              homepage = "https://github.com/xddxdd/never-gonna-rust";
              license = pkgs.lib.licenses.mit;
              mainProgram = "never-gonna";
              maintainers = with pkgs.lib.maintainers; [ xddxdd ];
            };
          };

          packages.default = config.packages.never-gonna;

          devShells.default = pkgs.mkShell {
            packages = with pkgs; [
              rustToolchain
              cargo-watch
              rust-analyzer
              alsa-utils
            ];
            RUST_SRC_PATH = "${rustToolchain}/lib/rustlib/src/rust/library";
          };
        };
    };
}
