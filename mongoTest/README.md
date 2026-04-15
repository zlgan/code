# MongoDB Change Stream DDD 项目

这是一个符合 DDD（领域驱动设计）架构的 .NET 10 Web API 项目，实现了 MongoDB Change Stream 监听框架。

## 项目结构

```
MongoChangeStream.DDD/
├── MongoChangeStream.DDD.Domain/          # 领域层
│   ├── Entities/                           # 实体
│   │   ├── BaseEntity.cs                  # 基础实体（包含领域事件）
│   │   └── Product.cs                    # 产品实体示例
│   └── Events/                             # 领域事件
│       ├── IDomainEvent.cs                 # 领域事件接口
│       └── DomainEventBase.cs             # 领域事件基类
├── MongoChangeStream.DDD.Application/       # 应用层
│   ├── Common/                             # 公共组件
│   │   └── Interfaces/                   # 接口
│   │       └── IChangeEventHandler.cs    # Change Stream 处理器接口
│   └── Handlers/                           # 处理器
│       └── ProductChangeStreamHandler.cs  # 产品 Change Stream 处理器示例
├── MongoChangeStream.DDD.Infrastructure/     # 基础设施层
│   ├── ChangeStream/                        # Change Stream 框架
│   │   ├── IMongoChangeStreamProcessor.cs  # Change Stream 处理器接口
│   │   └── MongoChangeStreamProcessor.cs   # Change Stream 处理器实现
│   ├── Persistence/                         # 持久化
│   │   ├── MongoSettings.cs             # MongoDB 配置
│   │   └── MongoDbContext.cs            # MongoDB 数据上下文
│   └── DependencyInjection/                 # 依赖注入
│       └── ServiceCollectionExtensions.cs   # 服务扩展方法
└── MongoChangeStream.DDD.API/             # API 层
    └── Program.cs                          # 应用程序入口
```

## 功能特性

### 1. DDD 架构
- **领域层**：包含实体、值对象、领域事件
- **应用层**：包含用例、处理器、CQRS 模式
- **基础设施层**：包含外部服务、持久化、Change Stream 框架
- **API 层**：包含 Web API 端点

### 2. MongoDB Change Stream 监听框架
- 基于 `BackgroundService` 实现
- 支持监听多个集合的变更
- 支持多种操作类型（Insert、Update、Delete、Replace）
- 自动重连机制
- 依赖注入支持

### 3. RESTful API
- 产品 CRUD 操作
- Swagger/OpenAPI 支持

## 配置

在 `appsettings.json` 中配置 MongoDB 连接：

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MongoChangeStreamDB"
  }
}
```

## 使用方法

### 1. 创建 Change Stream 处理器

```csharp
public class ProductChangeStreamHandler : IChangeEventHandler<Product>
{
    public string Name => "ProductChangeStreamHandler";

    public Task HandleInsertAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product inserted: {entity.Id} - {entity.Name}");
        return Task.CompletedTask;
    }

    public Task HandleUpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product updated: {entity.Id} - {entity.Name}");
        return Task.CompletedTask;
    }

    public Task HandleDeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product deleted: {entity.Id}");
        return Task.CompletedTask;
    }

    public Task HandleReplaceAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product replaced: {entity.Id} - {entity.Name}");
        return Task.CompletedTask;
    }
}
```

### 2. 注册 Change Stream 处理器

在 `Program.cs` 中注册：

```csharp
builder.Services.RegisterChangeStreamHandler<Product, ProductChangeStreamHandler>("Products");
```

### 3. 运行项目

```bash
dotnet run --project src/MongoChangeStream.DDD.API/MongoChangeStream.DDD.API.csproj
```

### 4. 测试 API

启动项目后，可以通过以下 API 端点进行测试：

- `POST /api/products` - 创建产品
- `GET /api/products` - 获取所有产品
- `GET /api/products/{id}` - 获取单个产品
- `PUT /api/products/{id}` - 更新产品
- `DELETE /api/products/{id}` - 删除产品

Swagger UI: `http://localhost:5000/swagger`

## MongoDB 要求

确保 MongoDB 作为副本集运行，Change Stream 才能正常工作：

```bash
# 启动单节点副本集
mongod --replSet "rs0" --dbpath "C:\data\db" --port 27017

# 初始化副本集（在 MongoDB shell 中）
rs.initiate()
```

## 技术栈

- .NET 10
- MongoDB Driver 3.7.1
- MediatR 14.1.0
- Swashbuckle.AspNetCore 10.1.7
