
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
│       └── Program.cs, Startup.cs, etc.
├── frontend/
│   └── src/components/ImpressionDashboard.jsx
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
