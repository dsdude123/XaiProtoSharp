# XaiProtoSharp

[![NuGet](https://img.shields.io/nuget/v/XaiProtoSharp)](https://www.nuget.org/packages/XaiProtoSharp)
[![License](https://img.shields.io/github/license/dsdude123/XaiProtoSharp)](LICENSE)
[![Build and Publish](https://github.com/dsdude123/XaiProtoSharp/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/dsdude123/XaiProtoSharp/actions/workflows/build-and-publish.yml)

Auto-generated C# gRPC client for the [xAI API](https://docs.x.ai/), built from protobuf definitions in [xai-org/xai-proto](https://github.com/xai-org/xai-proto).

> **Disclaimer:** This is an unofficial, community-maintained project. All library code is **auto-generated** from upstream protobuf definitions and is provided **as-is with no guarantee of functionality**. This project is not affiliated with or endorsed by xAI. Use at your own risk.

## Installation

Install via NuGet:

```bash
dotnet add package XaiProtoSharp
```

## Quick Start

```csharp
using Grpc.Core;
using Grpc.Net.Client;
using XaiApi;

// Create a gRPC channel
var channel = GrpcChannel.ForAddress("https://api.x.ai");
var client = new Chat.ChatClient(channel);

// Authenticate with your API key
var headers = new Metadata
{
    { "Authorization", $"Bearer {apiKey}" }
};

// Send a chat completion request
var request = new GetCompletionsRequest
{
    Model = "grok-4-1-fast-reasoning",
    Messages =
    {
        new Message
        {
            Content = { new Content { Text = "Hello, world!" } },
            Role = MessageRole.RoleUser
        }
    },
    MaxTokens = 100
};

var response = client.GetCompletion(request, headers);
foreach (var output in response.Outputs)
{
    Console.WriteLine(output.Message.Content);
}
```

## Supported Frameworks

- .NET 8.0
- .NET 9.0
- .NET 10.0

## Building from Source

**Prerequisites:** .NET 10.0 SDK, [Buf CLI](https://buf.build/docs/installation) (for code generation only)

```bash
# Build
dotnet build XaiProtoSharp.slnx

# Run tests (requires XAI_API_KEY environment variable)
dotnet test Xai.Protos.Test
```

To regenerate the gRPC client code from upstream protobuf definitions:

```bash
git clone https://github.com/xai-org/xai-proto.git xai-proto
buf generate xai-proto
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to contribute to this project.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.
