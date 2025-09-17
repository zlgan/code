// pages/inbound/index.js
const storage = require('../../utils/storage.js');

Page({
  data: {
    formData: {
      id: '',
      orderNo: '',
      type: '',
      date: '',
      supplier: {},
      product: {},
      quantity: '',
      price: '',
      remark: ''
    },
    inboundTypes: [],
    typeIndex: null,
    suppliers: [],
    supplierIndex: null,
    products: [],
    productIndex: null,
    totalAmount: '0.00'
  },

  onLoad() {
    // 生成单据号
    const orderNo = storage.generateOrderNo('RK');
    
    // 获取当前日期
    const today = new Date();
    const date = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(today.getDate()).padStart(2, '0')}`;

    // 加载基础数据
    const inboundTypes = storage.getData('inboundTypes');
    const suppliers = storage.getData('suppliers');
    const products = storage.getData('products').filter(p => p.enabled);

    this.setData({
      'formData.orderNo': orderNo,
      'formData.date': date,
      inboundTypes,
      suppliers,
      products
    });
  },

  // 选择入库类型
  onTypeChange(e) {
    const index = e.detail.value;
    this.setData({
      typeIndex: index,
      'formData.type': this.data.inboundTypes[index]
    });
  },

  // 选择日期
  onDateChange(e) {
    this.setData({
      'formData.date': e.detail.value
    });
  },

  // 选择供应商
  onSupplierChange(e) {
    const index = e.detail.value;
    this.setData({
      supplierIndex: index,
      'formData.supplier': this.data.suppliers[index]
    });
  },

  // 选择产品
  onProductChange(e) {
    const index = e.detail.value;
    const product = this.data.products[index];
    this.setData({
      productIndex: index,
      'formData.product': product,
      'formData.price': product.inboundPrice
    });
    this.calculateAmount();
  },

  // 输入数量
  onQuantityInput(e) {
    this.setData({
      'formData.quantity': e.detail.value
    });
    this.calculateAmount();
  },

  // 输入单价
  onPriceInput(e) {
    this.setData({
      'formData.price': e.detail.value
    });
    this.calculateAmount();
  },

  // 计算金额
  calculateAmount() {
    const { quantity, price } = this.data.formData;
    const amount = quantity && price ? (parseFloat(quantity) * parseFloat(price)).toFixed(2) : '0.00';
    this.setData({ totalAmount: amount });
  },

  // 提交表单
  submitForm(e) {
    const formData = e.detail.value;
    const { orderNo, type, date, supplier, product, quantity, price } = this.data.formData;

    // 表单验证
    if (!type) {
      wx.showToast({
        title: '请选择入库类型',
        icon: 'none'
      });
      return;
    }
    if (!date) {
      wx.showToast({
        title: '请选择入库日期',
        icon: 'none'
      });
      return;
    }
    if (!product.id) {
      wx.showToast({
        title: '请选择产品',
        icon: 'none'
      });
      return;
    }
    if (!quantity || quantity <= 0) {
      wx.showToast({
        title: '请输入正确的入库数量',
        icon: 'none'
      });
      return;
    }
    if (!price || price <= 0) {
      wx.showToast({
        title: '请输入正确的入库单价',
        icon: 'none'
      });
      return;
    }

    // 构建入库记录
    const inboundRecord = {
      id: storage.generateId(),
      orderNo,
      type,
      date,
      supplier,
      product: {
        id: product.id,
        name: product.name,
        code: product.code,
        unit: product.unit
      },
      quantity: parseFloat(quantity),
      price: parseFloat(price),
      amount: parseFloat(this.data.totalAmount),
      remark: formData.remark,
      createTime: new Date().toISOString()
    };

    // 保存入库记录
    const inboundRecords = storage.getData('inboundRecords');
    inboundRecords.push(inboundRecord);
    storage.saveData('inboundRecords', inboundRecords);

    // 更新产品库存
    const products = storage.getData('products');
    const productIndex = products.findIndex(p => p.id === product.id);
    if (productIndex > -1) {
      products[productIndex].stock = (products[productIndex].stock || 0) + parseFloat(quantity);
      storage.saveData('products', products);
    }

    wx.showToast({
      title: '入库成功',
      icon: 'success',
      success: () => {
        setTimeout(() => {
          wx.navigateBack();
        }, 1500);
      }
    });
  }
});