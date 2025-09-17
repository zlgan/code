// pages/profile/index.js
const storage = require('../../utils/storage.js')

Page({
  data: {
    userInfo: null,
    company: ''
  },

  onShow() {
    // 获取用户信息
    const userInfo = storage.get('userInfo')
    const company = storage.get('company')
    this.setData({
      userInfo,
      company
    })
  },

  // 处理登录
  handleLogin() {
    wx.getUserProfile({
      desc: '用于完善用户资料',
      success: (res) => {
        const userInfo = res.userInfo
        storage.set('userInfo', userInfo)
        this.setData({
          userInfo
        })
      },
      fail: (err) => {
        console.error('获取用户信息失败', err)
        wx.showToast({
          title: '获取用户信息失败',
          icon: 'none'
        })
      }
    })
  },

  // 导航到公司信息页面
  navigateToCompanyInfo() {
    wx.navigateTo({
      url: '/pages/profile/company/index'
    })
  },

  // 导航到系统设置页面
  navigateToSettings() {
    wx.navigateTo({
      url: '/pages/profile/settings/index'
    })
  },

  // 导航到关于我们页面
  navigateToAbout() {
    wx.navigateTo({
      url: '/pages/profile/about/index'
    })
  }
})