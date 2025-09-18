product #商品表
product_type #商品分类
product_unit #商品单位
supplier #供应商
inventory #库存
inventory_transaction #库存交易
stocktake #盘点单
stocktake_item #盘点单明细
transaction_type #交易类型 
                    #SALE_OUT (销售出库), USAGE_OUT (领用出库), STOCKTAKE_OUT (盘亏出库) 
                    #PURCHASE_IN (采购入库), RETURN_IN (退货入库), STOCKTAKE_IN (盘盈入库),

解决方案（正确设计）：必须有一个 inventory_transactions (库存流水表) 来记录每一次变化。

id	product_id	change	type	reference	operator	time
1	123	+100	PURCHASE	采购单PO100	张三	2023-10-01
2	123	-2	SALE	订单SO201	李四	2023-10-02
3	123	-1	SALE	订单SO202	李四	2023-10-02
...	...	...	...	...	...	...
99	123	-5	ADJUST	盘点调整	王五