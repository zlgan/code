// pages/profile/about/index.js
Page({
  data: {},

  // 复制文本到剪贴板
  handleCopyText(e) {
    const text = e.currentTarget.dataset.text
    wx.setClipboardData({
      data: text,
      success: () => {
        wx.showToast({
          title: '复制成功',
          icon: 'success'
        })
      }
    })
  }
})