// pages/outbound/list/index.js
const storage = require('../../../utils/storage.js');

Page({
  data: {
    outboundRecords: [],
    filteredRecords: [],
    outboundTypes: [],
    searchKey: '',
    showFilterPopup: false,
    hasFilter: false,
    filter: {
      type: '',
      startDate: '',
      endDate: ''
    }
  },

  onShow() {
    // 加载出库记录
    const outboundRecords = storage.getData('outboundRecords').sort((a, b) => {
      return new Date(b.createTime) - new Date(a.createTime);
    });
    const outboundTypes = storage.getData('outboundTypes');

    this.setData({
      outboundRecords,
      outboundTypes,
      filteredRecords: outboundRecords
    });

    this.applyFilters();
  },

  // 搜索
  onSearchInput(e) {
    this.setData({
      searchKey: e.detail.value
    });
    this.applyFilters();
  },

  // 显示筛选弹窗
  showFilter() {
    this.setData({
      showFilterPopup: true
    });
  },

  // 隐藏筛选弹窗
  hideFilter() {
    this.setData({
      showFilterPopup: false
    });
  },

  // 选择出库类型
  onTypeFilter(e) {
    const type = e.currentTarget.dataset.type;
    this.setData({
      'filter.type': this.data.filter.type === type ? '' : type
    });
  },

  // 选择开始日期
  onStartDateChange(e) {
    this.setData({
      'filter.startDate': e.detail.value
    });
  },

  // 选择结束日期
  onEndDateChange(e) {
    this.setData({
      'filter.endDate': e.detail.value
    });
  },

  // 重置筛选条件
  resetFilter() {
    this.setData({
      filter: {
        type: '',
        startDate: '',
        endDate: ''
      }
    });
  },

  // 应用筛选条件
  applyFilter() {
    this.hideFilter();
    this.applyFilters();
  },

  // 应用所有筛选条件
  applyFilters() {
    const { outboundRecords, searchKey, filter } = this.data;
    let filtered = [...outboundRecords];

    // 搜索关键词筛选
    if (searchKey) {
      filtered = filtered.filter(record => {
        return record.orderNo.toLowerCase().includes(searchKey.toLowerCase()) ||
               record.product.name.toLowerCase().includes(searchKey.toLowerCase()) ||
               record.product.code.toLowerCase().includes(searchKey.toLowerCase()) ||
               (record.customer && record.customer.toLowerCase().includes(searchKey.toLowerCase()));
      });
    }

    // 出库类型筛选
    if (filter.type) {
      filtered = filtered.filter(record => record.type === filter.type);
    }

    // 日期范围筛选
    if (filter.startDate) {
      filtered = filtered.filter(record => record.date >= filter.startDate);
    }
    if (filter.endDate) {
      filtered = filtered.filter(record => record.date <= filter.endDate);
    }

    // 更新筛选状态
    const hasFilter = !!(filter.type || filter.startDate || filter.endDate);

    this.setData({
      filteredRecords: filtered,
      hasFilter
    });
  },

  // 查看详情
  viewDetail(e) {
    const id = e.currentTarget.dataset.id;
    wx.navigateTo({
      url: `/pages/outbound/detail/index?id=${id}`
    });
  },

  // 新增出库
  navigateToAdd() {
    wx.navigateTo({
      url: '/pages/outbound/index'
    });
  }
});