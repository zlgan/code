// pages/outbound/detail/index.js
const storage = require('../../../utils/storage.js');
const util = require('../../../utils/util.js');

Page({
  data: {
    record: null,
    createTimeStr: ''
  },

  onLoad(options) {
    const { id } = options;
    const outboundRecords = storage.getData('outboundRecords');
    const record = outboundRecords.find(r => r.id === id);

    if (record) {
      // 格式化创建时间
      const createTimeStr = util.formatTime(new Date(record.createTime));

      this.setData({
        record,
        createTimeStr
      });
    } else {
      wx.showToast({
        title: '记录不存在',
        icon: 'error'
      });
      setTimeout(() => {
        wx.navigateBack();
      }, 1500);
    }
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
    wx.showActionSheet({
      itemList: ['分享到朋友圈', '分享给朋友'],
      success: (res) => {
        if (res.tapIndex === 0) {
          // 分享到朋友圈
          wx.showToast({
            title: '分享到朋友圈功能开发中',
            icon: 'none'
          });
        } else if (res.tapIndex === 1) {
          // 分享给朋友
          wx.showShareMenu({
            withShareTicket: true,
            menus: ['shareAppMessage']
          });
        }
      }
    });
  },

  // 分享给朋友
  onShareAppMessage() {
    const { record } = this.data;
    return {
      title: `出库单：${record.orderNo}`,
      path: `/pages/outbound/detail/index?id=${record.id}`
    };
  }
});