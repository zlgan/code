// pages/profile/company/index.js
const storage = require('../../../utils/storage.js')

Page({
  data: {
    company: {
      name: '',
      contact: '',
      phone: '',
      email: '',
      address: '',
      remark: ''
    }
  },

  onLoad() {
    // 获取公司信息
    const company = storage.get('company') || {}
    this.setData({
      company
    })
  },

  // 处理表单提交
  handleSubmit(e) {
    const formData = e.detail.value
    
    // 表单验证
    if (!formData.name.trim()) {
      wx.showToast({
        title: '请输入公司名称',
        icon: 'none'
      })
      return
    }

    if (!formData.contact.trim()) {
      wx.showToast({
        title: '请输入联系人',
        icon: 'none'
      })
      return
    }

    if (!formData.phone.trim()) {
      wx.showToast({
        title: '请输入联系电话',
        icon: 'none'
      })
      return
    }

    // 验证手机号格式
    if (!/^1\d{10}$/.test(formData.phone)) {
      wx.showToast({
        title: '请输入正确的手机号',
        icon: 'none'
      })
      return
    }

    // 验证邮箱格式（如果填写了）
    if (formData.email && !/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(formData.email)) {
      wx.showToast({
        title: '请输入正确的邮箱地址',
        icon: 'none'
      })
      return
    }

    // 保存公司信息
    storage.set('company', formData)

    wx.showToast({
      title: '保存成功',
      icon: 'success',
      duration: 2000,
      success: () => {
        // 延迟返回上一页
        setTimeout(() => {
          wx.navigateBack()
        }, 2000)
      }
    })
  }
})