// pages/product/list/index.js
const storage = require('../../../utils/storage.js');

Page({
  data: {
    products: [],
    filteredProducts: [],
    searchKey: '',
    showFilter: false,
    categories: [],
    categoryIndex: null,
    stockStatus: ['全部', '库存充足', '库存预警'],
    stockStatusIndex: 0,
    currentFilters: {
      category: '',
      stockStatus: '全部'
    }
  },

  onShow() {
    this.loadData();
  },

  // 加载数据
  loadData() {
    const products = storage.getData('products');
    const categories = storage.getData('categories');
    
    this.setData({ 
      products,
      filteredProducts: products,
      categories
    });

    this.applyFilters();
  },

  // 搜索输入
  onSearchInput(e) {
    this.setData({
      searchKey: e.detail.value
    });
    this.applyFilters();
  },

  // 显示筛选弹窗
  showFilterPopup() {
    this.setData({ showFilter: true });
  },

  // 隐藏筛选弹窗
  hideFilterPopup() {
    this.setData({ showFilter: false });
  },

  // 选择类别
  onCategoryChange(e) {
    this.setData({
      categoryIndex: e.detail.value,
      'currentFilters.category': this.data.categories[e.detail.value]
    });
  },

  // 选择库存状态
  onStockStatusChange(e) {
    this.setData({
      stockStatusIndex: e.detail.value,
      'currentFilters.stockStatus': this.data.stockStatus[e.detail.value]
    });
  },

  // 重置筛选
  resetFilter() {
    this.setData({
      categoryIndex: null,
      stockStatusIndex: 0,
      currentFilters: {
        category: '',
        stockStatus: '全部'
      }
    });
    this.applyFilters();
    this.hideFilterPopup();
  },

  // 应用筛选
  applyFilter() {
    this.applyFilters();
    this.hideFilterPopup();
  },

  // 应用所有筛选条件
  applyFilters() {
    let filtered = [...this.data.products];
    const { searchKey, currentFilters } = this.data;

    // 搜索关键词筛选
    if (searchKey) {
      filtered = filtered.filter(item => 
        item.name.toLowerCase().includes(searchKey.toLowerCase()) ||
        (item.code && item.code.toLowerCase().includes(searchKey.toLowerCase()))
      );
    }

    // 类别筛选
    if (currentFilters.category) {
      filtered = filtered.filter(item => item.category === currentFilters.category);
    }

    // 库存状态筛选
    if (currentFilters.stockStatus !== '全部') {
      if (currentFilters.stockStatus === '库存预警') {
        filtered = filtered.filter(item => item.stock <= item.warningStock);
      } else if (currentFilters.stockStatus === '库存充足') {
        filtered = filtered.filter(item => item.stock > item.warningStock);
      }
    }

    this.setData({ filteredProducts: filtered });
  },

  // 跳转到添加产品页面
  navigateToAdd() {
    wx.navigateTo({
      url: '/pages/product/add/index'
    });
  },

  // 编辑产品
  editProduct(e) {
    const { id } = e.currentTarget.dataset;
    wx.navigateTo({
      url: `/pages/product/add/index?id=${id}`
    });
  }
});