## product_category 产品类别
- cate_name (类别名称)
## supplier 供应商
- id
- supp_name (名称)
## product 产品
```json
{
  "_id": "product_001",
  "name": "无线蓝牙耳机",
  "sku": "P10001",
  "description": "高端无线蓝牙耳机",
  "category": "电子产品",
  "purchasePrice": 120.00,  // 默认入库单价（成本价）
  "retailPrice": 199.00,    // 默认出库单价（销售价）
  "currentAvgCost": 120.00, // 当前加权平均成本（由系统自动计算）
  "safetyStock": 10, //预警数量
  "createdAt": "2023-10-01T08:00:00Z",
  "updatedAt": "2023-10-27T14:30:00Z"
}
```
## inventory 库存
- product_id
- quantity
## inventory_transaction 库存流水
```json 
// 入库交易文档示例
{
  "_id": "txn_202310271430001",
  "type": "PURCHASE",        // 交易类型: PURCHASE, SALE, COST, ADJUSTMENT
  "productId": "product_001",
  "productInfo": {           // 关键：嵌套商品信息快照
    "name": "无线蓝牙耳机",
    "sku": "P10001",
    "category": "电子产品"
  },
  "quantityChange": 100,     // 数量变化(正数入库，负数出库)
  "unitPrice": 25.50,        // 交易单价(采购价/销售价)
  "unitCost": NULL,         // 成本价(出库时记录当时的成本快照)
  "totalValue": 2550.00,     // 总金额(quantityChange * unitPrice)
  "referenceNo": "PO-2023-1001", // 参考单号
  "operator": "张三",
  "notes": "向供应商A采购",
  "createdAt": "2023-10-27T14:30:00Z",
  "monthYear": "2023-10"     // 用于按月份查询的优化字段
}

// 出库交易文档示例
{
  "_id": "txn_202310271530001", 
  "type": "SALE",
  "productId": "product_001",
  "productInfo": {
    "name": "无线蓝牙耳机",
    "sku": "P10001",
    "category": "电子产品"
  },
  "quantityChange": -5,      // 负数表示出库
  "unitPrice": 39.90,        // 销售单价
  "unitCost": 23.33,         // 出库时的成本快照(系统自动计算)
  "totalValue": -199.50,     // 总金额(负数表示收入)
  "referenceNo": "SO-2023-2001",
  "operator": "李四",
  "notes": "销售给客户B",
  "createdAt": "2023-10-27T15:30:00Z",
  "monthYear": "2023-10"
}
// 费用交易文档示例
{
  "_id": "txn_202310281000001",
  "type": "COST",
  "description": "10月办公室租金", // 费用描述
  "unitPrice": 5000.00,       // 费用金额
  "totalValue": -5000.00,     // 负值表示支出
  "operator": "王五",
  "notes": "支付办公室租金",
  "createdAt": "2023-10-28T10:00:00Z",
  "monthYear": "2023-10"
}
```
## stocktake (盘点单表) 
- id : 盘点单号 (如: ST20231027-001)
- status : 状态 (如: DRAFT(草稿), COUNTING(盘点中), COMPLETED(已完成), CANCELLED(已取消))
- started_at : 盘点开始时间
- completed_at : 盘点完成/结束时间
- operator_id : 操作人员
- reviewer_id : 审核人员
- memo : 备注

## stocktake_items (盘点单项目表)
- id
- stocktake_id : 关联的盘点单号
- product_id : 商品ID
- expected_quantity : 系统账面数量 (盘点开始时从 inventory 表快照而来)
- counted_quantity : 实际清点数量 (盘点人员录入)
- difference : 差异数量 (计算得出： counted_quantity - expected_quantity)