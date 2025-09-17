// pages/document/index.js
const storage = require('../../utils/storage.js');

Page({
  data: {
    // 基础数据
    searchKey: '',
    activeTab: 'all',
    showFilterPopup: false,
    hasFilter: false,
    loading: false,
    hasMore: true,
    pageSize: 20,
    currentPage: 1,

    // 单据数据
    allDocuments: [],
    filteredDocuments: [],
    allCount: 0,
    inboundCount: 0,
    outboundCount: 0,

    // 筛选数据
    documentTypes: [
      { label: '入库单', value: 'inbound' },
      { label: '出库单', value: 'outbound' }
    ],
    inboundTypes: [],
    outboundTypes: [],
    currentTypes: [],
    filter: {
      documentType: '',
      type: '',
      startDate: '',
      endDate: ''
    }
  },

  onShow() {
    this.loadDocuments();
  },

  // 加载单据数据
  loadDocuments() {
    // 获取入库和出库记录
    const inboundRecords = storage.getData('inboundRecords') || [];
    const outboundRecords = storage.getData('outboundRecords') || [];
    const inboundTypes = storage.getData('inboundTypes') || [];
    const outboundTypes = storage.getData('outboundTypes') || [];

    // 转换为统一格式
    const allDocuments = [
      ...inboundRecords.map(record => ({
        ...record,
        documentType: 'inbound'
      })),
      ...outboundRecords.map(record => ({
        ...record,
        documentType: 'outbound'
      }))
    ].sort((a, b) => new Date(b.createTime) - new Date(a.createTime));

    // 统计数量
    const allCount = allDocuments.length;
    const inboundCount = inboundRecords.length;
    const outboundCount = outboundRecords.length;

    this.setData({
      allDocuments,
      inboundTypes,
      outboundTypes,
      currentTypes: [],
      allCount,
      inboundCount,
      outboundCount
    }, () => {
      this.applyFilters();
    });
  },

  // 搜索输入
  onSearchInput(e) {
    this.setData({
      searchKey: e.detail.value,
      currentPage: 1
    }, () => {
      this.applyFilters();
    });
  },

  // 切换标签
  switchTab(e) {
    const tab = e.currentTarget.dataset.tab;
    this.setData({
      activeTab: tab,
      currentPage: 1
    }, () => {
      this.applyFilters();
    });
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

  // 阻止冒泡
  stopPropagation() {},

  // 选择单据类型
  onDocumentTypeFilter(e) {
    const documentType = e.currentTarget.dataset.type;
    const currentTypes = documentType === 'inbound' ? this.data.inboundTypes : 
                        documentType === 'outbound' ? this.data.outboundTypes : [];
    
    this.setData({
      'filter.documentType': this.data.filter.documentType === documentType ? '' : documentType,
      'filter.type': '',
      currentTypes
    });
  },

  // 选择业务类型
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
        documentType: '',
        type: '',
        startDate: '',
        endDate: ''
      },
      currentTypes: []
    });
  },

  // 应用筛选条件
  applyFilter() {
    this.setData({
      showFilterPopup: false,
      currentPage: 1
    }, () => {
      this.applyFilters();
    });
  },

  // 应用所有筛选条件
  applyFilters() {
    const { 
      allDocuments, 
      activeTab, 
      searchKey, 
      filter,
      currentPage,
      pageSize
    } = this.data;

    let filtered = [...allDocuments];

    // 标签筛选
    if (activeTab === 'inbound') {
      filtered = filtered.filter(doc => doc.documentType === 'inbound');
    } else if (activeTab === 'outbound') {
      filtered = filtered.filter(doc => doc.documentType === 'outbound');
    }

    // 搜索关键词筛选
    if (searchKey) {
      const key = searchKey.toLowerCase();
      filtered = filtered.filter(doc => {
        return doc.orderNo.toLowerCase().includes(key) ||
               doc.product.name.toLowerCase().includes(key) ||
               doc.product.code.toLowerCase().includes(key) ||
               (doc.supplier && doc.supplier.toLowerCase().includes(key)) ||
               (doc.customer && doc.customer.toLowerCase().includes(key));
      });
    }

    // 单据类型筛选
    if (filter.documentType) {
      filtered = filtered.filter(doc => doc.documentType === filter.documentType);
    }

    // 业务类型筛选
    if (filter.type) {
      filtered = filtered.filter(doc => doc.type === filter.type);
    }

    // 日期范围筛选
    if (filter.startDate) {
      filtered = filtered.filter(doc => doc.date >= filter.startDate);
    }
    if (filter.endDate) {
      filtered = filtered.filter(doc => doc.date <= filter.endDate);
    }

    // 分页处理
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    const hasMore = filtered.length > end;

    this.setData({
      filteredDocuments: filtered.slice(0, end),
      hasMore,
      hasFilter: !!(filter.documentType || filter.type || filter.startDate || filter.endDate)
    });
  },

  // 加载更多
  loadMore() {
    if (this.data.loading || !this.data.hasMore) return;

    this.setData({
      loading: true,
      currentPage: this.data.currentPage + 1
    }, () => {
      this.applyFilters();
      this.setData({
        loading: false
      });
    });
  },

  // 查看详情
  viewDetail(e) {
    const { type, id } = e.currentTarget.dataset;
    wx.navigateTo({
      url: `/pages/${type}/detail/index?id=${id}`
    });
  }
});