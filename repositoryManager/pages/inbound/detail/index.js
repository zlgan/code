// pages/inbound/detail/index.js
const storage = require('../../../utils/storage.js');

Page({
  data: {
    record: null,
    createTimeStr: ''
  },

  onLoad(options) {
    const { id } = options;
    if (!id) {
      wx.showToast({
        title: '参数错误',
        icon: 'error'
      });
      setTimeout(() => {
        wx.navigateBack();
      }, 1500);
      return;
    }

    // 获取入库记录
    const inboundRecords = storage.getData('inboundRecords');
    const record = inboundRecords.find(r => r.id === id);
    if (!record) {
      wx.showToast({
        title: '记录不存在',
        icon: 'error'
      });
      setTimeout(() => {
        wx.navigateBack();
      }, 1500);
      return;
    }

    // 格式化创建时间
    const createTime = new Date(record.createTime);
    const createTimeStr = `${createTime.getFullYear()}-${String(createTime.getMonth() + 1).padStart(2, '0')}-${String(createTime.getDate()).padStart(2, '0')} ${String(createTime.getHours()).padStart(2, '0')}:${String(createTime.getMinutes()).padStart(2, '0')}`;

    this.setData({
      record,
      createTimeStr
    });
  },

  // 打印单据
  onPrint() {
    wx.showToast({
      title: '打印功能开发中',
      icon: 'none'
    });
  },

  // 分享
  onShare() {
    wx.showToast({
      title: '分享功能开发中',
      icon: 'none'
    });
  },

  // 分享到朋友圈
  onShareTimeline() {
    const { record } = this.data;
    return {
      title: `入库单：${record.orderNo}`,
      query: `id=${record.id}`
    };
  },

  // 分享给朋友
  onShareAppMessage() {
    const { record } = this.data;
    return {
      title: `入库单：${record.orderNo}`,
      path: `/pages/inbound/detail/index?id=${record.id}`
    };
  }
});