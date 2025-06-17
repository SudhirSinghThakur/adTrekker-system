
# Ad Impression Service

A full-stack serverless application to collect and visualize ad impression data using AWS and .NET.

## 🚀 Tech Stack
- **Backend**: ASP.NET Core, AWS Lambda, S3, DynamoDB
- **Frontend**: React + Vite
- **Infrastructure**: AWS SAM / Lambda Tools CLI

---

## 📁 Folder Structure
```
repo/
├── backend/
│   └── AdImpressionService/
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       ├── Tests/
│       ├── LambdaEntryPoint.cs
│       ├── aws-lambda-tools-defaults.json
│       ├── .gitignore
│       └── Program.cs, Startup.cs, etc.
├── frontend/
│   ├── src/components/ImpressionDashboard.jsx
│   └── .gitignore
├── README.md
```

---

## ⚙️ Backend Setup
1. Install .NET 8 SDK
2. Navigate to `backend/AdImpressionService`
3. Run locally:
```bash
dotnet run
```
4. Deploy to AWS Lambda:
```bash
dotnet lambda deploy-serverless
```

### 📄 `LambdaEntryPoint.cs`

#### ℹ️ Why We Use `Amazon.Lambda.AspNetCoreServer`
In traditional ASP.NET Core apps, a web server like Kestrel handles HTTP requests. However, AWS Lambda doesn’t run a web server. 

To run ASP.NET Core inside AWS Lambda, we use the `Amazon.Lambda.AspNetCoreServer` NuGet package. It acts as a bridge that transforms incoming AWS API Gateway requests into ASP.NET Core compatible HTTP context.

The `LambdaEntryPoint.cs` file is the Lambda function handler and initializes the application using the Startup class.

✅ This setup enables serverless hosting of ASP.NET Core Web APIs inside AWS Lambda.
```csharp
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<Startup>();
    }
}
```

### 📄 `aws-lambda-tools-defaults.json`

#### ℹ️ What is `aws-lambda-tools-defaults.json`?
This file is used by the AWS Lambda .NET CLI (`Amazon.Lambda.Tools`) to configure and automate deployment of your Lambda function.

It includes settings like:
- Lambda function name and runtime
- Memory size, timeout, region
- Entry point handler (`FunctionHandlerAsync`)

✅ Instead of manually setting these values in the AWS Console, just run:
```bash
dotnet lambda deploy-serverless
```
…and this file will be used to deploy your function automatically.
```json
{
  "function-name": "AdImpressionAPI",
  "function-handler": "AdImpressionService::AdImpressionService.LambdaEntryPoint::FunctionHandlerAsync",
  "framework": "net8.0",
  "function-runtime": "dotnet8",
  "function-memory-size": 512,
  "function-timeout": 30,
  "region": "us-east-1",
  "configuration": "Release"
}
```

---

## ✅ Unit Tests
Run backend tests:
```bash
cd backend/AdImpressionService
dotnet test
```

---

## 🧪 Sample Unit Tests
### `S3UploaderTests.cs`
```csharp
[Fact]
public async Task UploadAsync_ValidInput_UploadsToS3()
{
    var s3Mock = new Mock<IAmazonS3>();
    var uploader = new S3Uploader(s3Mock.Object);

    var impression = new AdImpression { ImpressionId = "test-id", CampaignId = "cmp1", Timestamp = DateTime.UtcNow };

    await uploader.UploadAsync(impression);

    s3Mock.Verify(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()), Times.Once);
}
```

### `AdImpressionControllerTests.cs`
```csharp
[Fact]
public async Task Post_ValidImpression_ReturnsOk()
{
    var s3Mock = new Mock<S3Uploader>();
    var loggerMock = new Mock<DynamoLogger>();

    var controller = new AdImpressionController(s3Mock.Object, loggerMock.Object);
    var result = await controller.Post(new AdImpression { ImpressionId = "1", CampaignId = "c1", Timestamp = DateTime.UtcNow });

    Assert.IsType<OkObjectResult>(result);
}
```

---

## 🌐 Frontend Setup
1. Navigate to `frontend/`
2. Install dependencies:
```bash
npm install
```
3. Run app:
```bash
npm run dev
```

---

## 📊 ImpressionDashboard.jsx
```jsx
import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function ImpressionDashboard() {
  const [impressions, setImpressions] = useState([]);

  useEffect(() => {
    axios.get('/api/v1/impressions')
      .then(res => setImpressions(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Ad Impressions</h2>
      <table className="min-w-full bg-white">
        <thead>
          <tr>
            <th>ID</th><th>Campaign</th><th>Timestamp</th><th>Location</th>
          </tr>
        </thead>
        <tbody>
          {impressions.map(impression => (
            <tr key={impression.impressionId}>
              <td>{impression.impressionId}</td>
              <td>{impression.campaignId}</td>
              <td>{new Date(impression.timestamp).toLocaleString()}</td>
              <td>{impression.location}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

---

## 🛡 Security
- IAM Roles with least privilege
- CORS and Input Validation enforced on API

---

## 📬 Contact
Built by Sudhir Singh Thakur · [LinkedIn](https://www.linkedin.com/in/sudhir-singh-thakur-55892196/) · [GitHub](https://github.com/SudhirSinghThakur)
