// pages/profile/settings/index.js
const storage = require('../../../utils/storage.js')
const util = require('../../../utils/util.js')

Page({
  data: {
    settings: {
      notification: true,
      sound: true,
      lastBackup: ''
    },
    hasLogin: false
  },

  onLoad() {
    // 获取设置信息
    const settings = storage.get('settings') || {
      notification: true,
      sound: true
    }
    const userInfo = storage.get('userInfo')
    
    this.setData({
      settings,
      hasLogin: !!userInfo
    })
  },

  // 处理消息通知开关
  handleNotificationChange(e) {
    const settings = this.data.settings
    settings.notification = e.detail.value
    this.setData({ settings })
    storage.set('settings', settings)
  },

  // 处理声音提醒开关
  handleSoundChange(e) {
    const settings = this.data.settings
    settings.sound = e.detail.value
    this.setData({ settings })
    storage.set('settings', settings)
  },

  // 处理数据备份
  handleBackup() {
    wx.showLoading({
      title: '备份中...'
    })

    // 获取所有需要备份的数据
    const backupData = {
      products: storage.get('products') || [],
      suppliers: storage.get('suppliers') || [],
      inboundRecords: storage.get('inboundRecords') || [],
      outboundRecords: storage.get('outboundRecords') || [],
      company: storage.get('company') || {},
      settings: this.data.settings
    }

    // 将数据转换为字符串
    const backupStr = JSON.stringify(backupData)

    // 保存备份时间
    const settings = this.data.settings
    settings.lastBackup = util.formatTime(new Date())
    this.setData({ settings })
    storage.set('settings', settings)

    // 模拟备份过程
    setTimeout(() => {
      wx.hideLoading()
      wx.showToast({
        title: '备份成功',
        icon: 'success'
      })
    }, 1500)
  },

  // 处理清除缓存
  handleClearCache() {
    wx.showModal({
      title: '提示',
      content: '确定要清除缓存吗？清除后将无法恢复。',
      success: (res) => {
        if (res.confirm) {
          wx.showLoading({
            title: '清除中...'
          })

          // 清除缓存数据
          storage.clear()

          // 重置设置
          const settings = {
            notification: true,
            sound: true
          }
          this.setData({
            settings,
            hasLogin: false
          })
          storage.set('settings', settings)

          setTimeout(() => {
            wx.hideLoading()
            wx.showToast({
              title: '清除成功',
              icon: 'success',
              success: () => {
                // 返回首页
                wx.switchTab({
                  url: '/pages/index/index'
                })
              }
            })
          }, 1500)
        }
      }
    })
  },

  // 处理检查更新
  handleCheckUpdate() {
    wx.showLoading({
      title: '检查中...'
    })

    // 模拟检查更新
    setTimeout(() => {
      wx.hideLoading()
      wx.showToast({
        title: '已是最新版本',
        icon: 'success'
      })
    }, 1500)
  },

  // 处理退出登录
  handleLogout() {
    wx.showModal({
      title: '提示',
      content: '确定要退出登录吗？',
      success: (res) => {
        if (res.confirm) {
          // 清除用户信息
          storage.remove('userInfo')
          
          this.setData({
            hasLogin: false
          })

          wx.showToast({
            title: '已退出登录',
            icon: 'success',
            success: () => {
              // 返回我的页面
              wx.navigateBack()
            }
          })
        }
      }
    })
  }
})