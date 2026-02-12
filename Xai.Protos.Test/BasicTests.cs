using Grpc.Core;
using Grpc.Net.Client;
using XaiApi;
using Xunit.Abstractions;

namespace Xai.Protos.Test;

public class BasicTests(ITestOutputHelper testOutputHelper)
{
    private readonly string apiKey = System.Environment.GetEnvironmentVariable("XAI_API_KEY") ?? 
                                     throw new System.InvalidOperationException("Environment variable XAI_API_KEY is not set.");

    [Fact]
    public void TestChat()
    {
        var grpcChannel = GrpcChannel.ForAddress("https://api.x.ai");
        var chatClient = new Chat.ChatClient(grpcChannel);
        
        Metadata headers = new Metadata
        {
            { "Authorization", $"Bearer {apiKey}" } 
        };

        var testRequest = new GetCompletionsRequest
        {
            Model = "grok-4-1-fast-reasoning",
            Messages =
            {
                new Message
                {
                    Content =
                    {
                        new Content
                        {
                            Text = "What is the meaning of life?"
                        }
                    }, 
                    Role = MessageRole.RoleUser
                }
            },
            MaxTokens = 100,
        };

        var response = chatClient.GetCompletion(testRequest, headers);
        Assert.NotNull(response);
        foreach (var output in response.Outputs)
        {
            testOutputHelper.WriteLine(output.Message.Content);
        }
        
        // Check that some output has non-empty text
        bool anyHasText = Enumerable.Any(response.Outputs, output => 
            !String.IsNullOrEmpty(output.Message.Content));
        
        Assert.True(anyHasText, "No output contains text.");
    }

    [Fact]
    public void TestImage()
    {
        var grpcChannel = GrpcChannel.ForAddress("https://api.x.ai");
        var imageClient = new Image.ImageClient(grpcChannel);

        Metadata headers = new Metadata
        {
            { "Authorization", $"Bearer {apiKey}" }
        };
        
        var testRequest = new GenerateImageRequest
        {
            Model = "grok-imagine-image",
            Prompt = "Capybara invasion of Seattle",
            N = 1,
            Format = ImageFormat.ImgFormatUrl
        };
        
        var response = imageClient.GenerateImage(testRequest, headers);
        Assert.NotNull(response);
        Assert.Single(response.Images);
        var image = response.Images[0];
        Assert.NotNull(image);
        testOutputHelper.WriteLine($"Image URL: {image.Url}");
        Assert.False(string.IsNullOrWhiteSpace(image.Url), "Image URL is null or empty.");

        bool validUrl = Uri.TryCreate(image.Url, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        Assert.True(validUrl, $"Image URL is not a valid http/https URL: {image.Url}");
    }

    [Fact]
    public void TestVideo()
    {
        var grpcChannel = GrpcChannel.ForAddress("https://api.x.ai");
        var videoClient = new Video.VideoClient(grpcChannel);
        
        Metadata headers = new Metadata
        {
            { "Authorization", $"Bearer {apiKey}" }
        };
        
        var testRequest = new GenerateVideoRequest
        {
            Model = "grok-imagine-video",
            Prompt = "Capybara invasion of Seattle",
            Duration = 1
        };
        
        var response = videoClient.GenerateVideo(testRequest, headers);
        Assert.NotNull(response);

        var deferredRequest = new GetDeferredVideoRequest
        {
            RequestId = response.RequestId
        };

        var deferredResponse = videoClient.GetDeferredVideo(deferredRequest, headers);
        while (deferredResponse.Status == DeferredStatus.Pending)
        {
            testOutputHelper.WriteLine("Video is still processing...");
            Thread.Sleep(5000); // Wait for 5 seconds before checking again
            deferredResponse = videoClient.GetDeferredVideo(deferredRequest, headers);
        }
        
        Assert.Equal(DeferredStatus.Done, deferredResponse.Status);
        Assert.NotNull(deferredResponse.Response.Video);

        var videoUrl = deferredResponse.Response.Video.Url;
        testOutputHelper.WriteLine($"Video URL: {videoUrl}");
        Assert.False(string.IsNullOrWhiteSpace(videoUrl), "Video URL is null or empty.");

        bool validUrl = Uri.TryCreate(videoUrl, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        Assert.True(validUrl, $"Video URL is not a valid http/https URL: {videoUrl}");
    }
}
