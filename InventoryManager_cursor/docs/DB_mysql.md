## product_category 产品类别
- cate_name (类别名称)
## supplier 供应商
- id
- supp_name (名称)
## product 产品
- id
- sku（商品库存单位编码(唯一)）
- name 
- category_id  (类别id)
- supp_id (供应商id)
- current_avg_cost (当前平均成本) 每次入库采用加权平均法重新计算，计算方法：（当前总价值+入库总价值）/（当前数量+入库数量）
- updated_at 
- created_at 
## inventory 库存
- product_id
- quantity
## inventory_transaction 库存流水
- id 
- product_id 
- quantity_change (变化数量，正数表示入库)
- quantity (更新后的库存)
- unit_price（采购单价或出库单价）
- unit_cost=（记录此次出库时所使用的成本价，也就是product.current_avg_cost）
- transaction_type (交易类型，如 'PURCHASE_IN', 'RETURN_IN')
- reference_id (关联单号，如采购单ID purchase_orders.id)
- operator (操作员)
- created_at (操作时间)
- notes （备注）
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