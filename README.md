
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

## 🔁 End-to-End Flow Summary
Frontend (or System)
   ⬇️ POST /api/v1/impressions
.NET Controller (validates and delegates)
   ⬇️ Save raw JSON to S3
   ⬇️ Save metadata to DynamoDB
   ✅ Respond 200 OK

⬆️ Later, GET /api/v1/impressions
⬅️ JSON list from DynamoDB
⬅️ Displayed in React dashboard

---

## 🛡 Security
- IAM Roles with least privilege
- CORS and Input Validation enforced on API

---

## 📘 AWS Lambda Learning Path

A progressive guide to help you become proficient in building serverless apps using AWS Lambda and .NET.

### 🚀 Stage 1: Beginner – Understanding the Basics
- **What is Lambda**: A serverless compute service that runs your C# code on demand.
- **Entry Point**: Your C# method `FunctionHandler()` is invoked by AWS.
```csharp
public class Function {
    public string FunctionHandler(string input, ILambdaContext context) {
        return $"Hello {input} from Lambda!";
    }
}
```
- **Deploy**: Use `dotnet lambda deploy-function MyFunctionName`

---

### 🧰 Stage 2: Intermediate – Web APIs & Dependency Injection
- Use ASP.NET Core APIs in Lambda via `Amazon.Lambda.AspNetCoreServer`
- Register AWS services using:
```csharp
builder.Services.AddAWSService<IAmazonS3>();
```
- Deploy with `dotnet lambda deploy-serverless`

---

### ⚙️ Stage 3: Configuration & Deployment
- Use `aws-lambda-tools-defaults.json` to define:
  - Runtime: `dotnet8`
  - Memory, Timeout, Region, Handler
```bash
# CLI Deployment
$ dotnet lambda deploy-serverless
```

---

### 📊 Stage 4: Advanced Lambda Scenarios
- **S3 Trigger**
```csharp
public async Task FunctionHandler(S3Event evnt, ILambdaContext context) {
    foreach (var record in evnt.Records) {
        var key = record.S3.Object.Key;
    }
}
```
- **SQS Batch Processing**
```csharp
public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context) {
    foreach (var message in evnt.Records) {
        var payload = message.Body;
    }
}
```

---

### 🔐 Stage 5: Best Practices
| Practice                     | Why                                      |
|-----------------------------|-------------------------------------------|
| Stateless                   | Enables scaling across many containers    |
| Environment Variables       | Externalize config                        |
| Logging with CloudWatch     | Debug and monitor                         |
| IAM Roles                   | Secure access to AWS services             |
| Minimize Cold Start         | Avoid heavy libraries                     |

---

### 🔁 Tools You Should Know
- `Amazon.Lambda.Tools` – CLI for deploying Lambda
- `Amazon.Lambda.AspNetCoreServer` – bridge ASP.NET and Lambda
- `AWSSDK.*` – official AWS service SDKs
- CloudWatch, IAM, API Gateway, DynamoDB, S3

---

## 📬 Contact
Built by Sudhir Singh Thakur · [LinkedIn](https://www.linkedin.com/in/sudhir-singh-thakur-55892196/) · [GitHub](https://github.com/SudhirSinghThakur)
