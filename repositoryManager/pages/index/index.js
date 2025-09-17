// index.js
const storage = require('../../utils/storage.js');

Page({
  data: {
    stats: {
      productCount: 0,
      warningCount: 0,
      todayInbound: 0,
      todayOutbound: 0
    }
  },

  onShow() {
    this.loadStats();
  },

  // 加载统计数据
  loadStats() {
    const products = storage.getData('products');
    const inboundRecords = storage.getData('inboundRecords');
    const outboundRecords = storage.getData('outboundRecords');
    
    // 获取今天的开始时间
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    // 计算统计数据
    const stats = {
      productCount: products.length,
      warningCount: products.filter(p => p.stock <= p.warningStock).length,
      todayInbound: inboundRecords.filter(r => new Date(r.date) >= today).length,
      todayOutbound: outboundRecords.filter(r => new Date(r.date) >= today).length
    };

    this.setData({ stats });
  },

  // 页面导航
  navigateTo(e) {
    const { path } = e.currentTarget.dataset;
    wx.navigateTo({
      url: path
    });
  }
});
