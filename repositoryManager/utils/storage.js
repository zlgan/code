// 本地存储工具模块

// 默认数据结构
const defaultData = {
  products: [], // 产品列表
  suppliers: [], // 供应商列表
  customers: [], // 客户列表
  inboundRecords: [], // 入库记录
  outboundRecords: [], // 出库记录
  categories: ['食品', '饮料', '日用品', '其他'], // 产品类别
  units: ['个', '箱', '件', '千克', '升'], // 产品单位
  inboundTypes: ['采购入库', '退货入库', '其他入库'], // 入库类型
  outboundTypes: ['销售出库', '退货出库', '其他出库'], // 出库类型
};

// 初始化本地存储
const initStorage = () => {
  Object.keys(defaultData).forEach(key => {
    if (!wx.getStorageSync(key)) {
      wx.setStorageSync(key, defaultData[key]);
    }
  });
};

// 获取数据
const getData = (key) => {
  return wx.getStorageSync(key) || defaultData[key] || [];
};

// 保存数据
const saveData = (key, data) => {
  wx.setStorageSync(key, data);
};

// 生成唯一ID
const generateId = () => {
  return Date.now().toString(36) + Math.random().toString(36).substr(2);
};

// 生成单据号
const generateOrderNo = (prefix) => {
  const date = new Date();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const random = Math.floor(Math.random() * 1000).toString().padStart(3, '0');
  return `${prefix}${year}${month}${day}${random}`;
};

module.exports = {
  initStorage,
  getData,
  saveData,
  generateId,
  generateOrderNo,
};