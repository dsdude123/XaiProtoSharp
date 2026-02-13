# Contributing to XaiProtoSharp

Thank you for your interest in contributing to XaiProtoSharp! This document provides guidelines for contributing to this project.

## Scope of Contributions

The `Xai.Protos/` directory contains **auto-generated code** and should not be modified by hand. Changes to generated code are made by updating the code generation pipeline, not by editing files directly.

Contributions are welcome in the following areas:

- **Tests** (`Xai.Protos.Test/`)
- **CI/CD workflows** (`.github/workflows/`)
- **Documentation** (`README.md`, `CONTRIBUTING.md`, etc.)
- **Build tooling** (`buf.gen.yaml`, `.csproj` files)

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Buf CLI](https://buf.build/docs/installation) (only needed for code regeneration)
- An xAI API key (only needed for running integration tests)

### Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/dsdude123/XaiProtoSharp.git
   cd XaiProtoSharp
   ```

2. Build the solution:
   ```bash
   dotnet build XaiProtoSharp.slnx
   ```

3. Run tests (requires `XAI_API_KEY` environment variable):
   ```bash
   dotnet test Xai.Protos.Test
   ```

## Submitting Changes

1. Fork the repository and create a branch for your changes.
2. Make your changes and ensure the project builds without errors.
3. Add or update tests if applicable.
4. Submit a pull request with a clear description of your changes.

## Reporting Issues

When opening an issue, please ensure it relates to **this project** (the C# wrapper, packaging, tests, CI, or documentation). Issues with upstream protobuf definitions or xAI API behavior should be reported to [xai-org/xai-proto](https://github.com/xai-org/xai-proto) or xAI directly.