# Kafka 實作練習專案

這是一個使用 .NET 實作 Kafka 訊息發佈/訂閱模式的示範專案，同時整合了完整的監控系統。

## 專案概述

本專案展示了如何使用 .NET 與 Apache Kafka 進行訊息傳遞，並使用 Prometheus 和 Grafana 來監控 Kafka 叢集的運行狀況。專案包含以下主要組件：

1. **Kafka 生產者**：用於發送訊息到 Kafka 主題
2. **Kafka 消費者**：用於從 Kafka 主題接收訊息
3. **監控系統**：包含 Kafka Exporter、Prometheus 和 Grafana，提供全面的監控儀表板

## 技術堆棧

- **開發語言**：C# / .NET 8.0
- **訊息佇列**：Apache Kafka
- **監控工具**：
  - Kafka Exporter：用於收集 Kafka 指標
  - Prometheus：時間序列資料庫，用於存儲指標數據
  - Grafana：視覺化儀表板，用於展示監控數據

## 專案結構

```
KafkaPractice/
│
├── src/                           # 原始碼目錄
│   ├── KafkaPractice.Publisher/   # Kafka 訊息發布者應用程式
│   └── KafkaPractice.Consumer/    # Kafka 訊息消費者應用程式
│
└── Infra/                         # 基礎設施配置
    ├── docker-compose.yml         # Docker Compose 配置文件
    └── prometheus/                # Prometheus 配置目錄
        └── prometheus.yml         # Prometheus 配置檔案
```

## 快速開始

### 前置條件

- 安裝 .NET 8.0 SDK
- 安裝 Docker 和 Docker Compose

### 啟動基礎設施

在 `Infra` 目錄下執行以下命令，啟動所有必要的服務（Kafka、Zookeeper、Prometheus、Grafana 等）：

```bash
cd Infra
docker-compose up -d
```

### 運行消費者應用程式

在專案根目錄執行以下命令，啟動消費者應用程式：

```bash
cd src/KafkaPractice.Consumer
dotnet run
```

消費者會連接到 Kafka 並開始等待接收訊息。

### 運行生產者應用程式

在專案根目錄執行以下命令，啟動生產者應用程式以產生測試訊息：

```bash
cd src/KafkaPractice.Publisher
dotnet run
```

生產者會向 `test-topic` 主題發送測試訊息。

## 消息傳遞保證

本專案實現了 Kafka 的 "at least once" (至少一次) 消息傳遞語義，這是一種確保消息不會丟失的機制。

### At Least Once 傳遞

- **核心概念**：確保每條消息至少被處理一次，寧可重複處理也不遺漏任何消息
  
- **實現方式**：
  - 先處理消息，成功後才提交偏移量 (`consumer.StoreOffset(result)`)
  - 如果處理過程中失敗或消費者崩潰，重啟後將重新消費未確認的消息
  
- **優缺點**：
  - ✓ 確保消息被 `成功處理` 一次
  - ✗ 導致 `重複消費`
  
- **最佳實踐**：
  - 消費者應實現冪等性處理 (同一消息處理多次結果相同)
  - 或實現去重機制 (識別並跳過重複消息)
  
- **適用場景**：
  - 金融交易系統

### 監控系統

系統啟動後，可以透過以下網址訪問監控工具：

- **Grafana**：http://localhost:3000 （預設帳號/密碼：admin/admin）
- **Prometheus**：http://localhost:9090
- **Kafka Exporter 指標**：http://localhost:9308/metrics

在 Grafana 中，您可以導入 Kafka 的預設儀表板（ID: 7589）或根據需求創建自定義儀表板。

## 故障排除

若儀表板顯示 "No data"，請確認：

1. Kafka 服務正常運行
2. Kafka Exporter 能正常連接到 Kafka
3. Prometheus 能成功從 Kafka Exporter 抓取指標
4. Grafana 使用的 Prometheus 數據源設定正確
5. 儀表板查詢語法正確且時間範圍設定合適

## 結語

這個專案展示了使用 .NET 與 Kafka 進行訊息傳遞，以及如何建立完整的監控系統。透過這個架構，您可以實現高效的事件驅動應用程式，同時擁有強大的可觀測性工具來監控系統的健康狀況。