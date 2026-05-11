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
          };

          packages.default = config.packages.never-gonna;

          devShells.default = pkgs.mkShell {
            packages = with pkgs; [
              rustToolchain
              cargo-watch
              rust-analyzer
            ];
            RUST_SRC_PATH = "${rustToolchain}/lib/rustlib/src/rust/library";
          };
        };
    };
}