// pages/product/add/index.js
const storage = require('../../../utils/storage.js');

Page({
  data: {
    formData: {
      id: '',
      image: '',
      name: '',
      code: '',
      category: '',
      unit: '',
      specification: '',
      supplier: {},
      inboundPrice: '',
      outboundPrice: '',
      warningStock: '',
      enabled: true,
      remark: '',
      stock: 0
    },
    categories: [],
    categoryIndex: null,
    units: [],
    unitIndex: null,
    suppliers: [],
    supplierIndex: null,
    primaryColor: '#3A7BF9'
  },

  onLoad(options) {
    // 加载基础数据
    const categories = storage.getData('categories');
    const units = storage.getData('units');
    const suppliers = storage.getData('suppliers');
    
    this.setData({ categories, units, suppliers });

    // 如果是编辑模式，加载产品数据
    if (options.id) {
      const products = storage.getData('products');
      const product = products.find(p => p.id === options.id);
      if (product) {
        this.setData({
          formData: product,
          categoryIndex: categories.findIndex(c => c === product.category),
          unitIndex: units.findIndex(u => u === product.unit),
          supplierIndex: suppliers.findIndex(s => s.id === product.supplier.id)
        });
      }
    }
  },

  // 选择图片
  chooseImage() {
    wx.chooseImage({
      count: 1,
      sizeType: ['compressed'],
      sourceType: ['album', 'camera'],
      success: (res) => {
        this.setData({
          'formData.image': res.tempFilePaths[0]
        });
      }
    });
  },

  // 选择类别
  onCategoryChange(e) {
    const index = e.detail.value;
    this.setData({
      categoryIndex: index,
      'formData.category': this.data.categories[index]
    });
  },

  // 选择单位
  onUnitChange(e) {
    const index = e.detail.value;
    this.setData({
      unitIndex: index,
      'formData.unit': this.data.units[index]
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

  // 启用状态变更
  onEnabledChange(e) {
    this.setData({
      'formData.enabled': e.detail.value
    });
  },

  // 提交表单
  submitForm(e) {
    const formData = e.detail.value;
    const { image, category, unit, supplier, enabled } = this.data.formData;
    
    // 表单验证
    if (!formData.name) {
      wx.showToast({
        title: '请输入产品名称',
        icon: 'none'
      });
      return;
    }
    if (!category) {
      wx.showToast({
        title: '请选择产品类别',
        icon: 'none'
      });
      return;
    }
    if (!unit) {
      wx.showToast({
        title: '请选择产品单位',
        icon: 'none'
      });
      return;
    }

    // 构建产品数据
    const productData = {
      ...this.data.formData,
      ...formData,
      image,
      category,
      unit,
      supplier,
      enabled,
      inboundPrice: formData.inboundPrice ? parseFloat(formData.inboundPrice) : 0,
      outboundPrice: formData.outboundPrice ? parseFloat(formData.outboundPrice) : 0,
      warningStock: formData.warningStock ? parseInt(formData.warningStock) : 0
    };

    // 保存产品
    const products = storage.getData('products');
    if (productData.id) {
      // 更新产品
      const index = products.findIndex(p => p.id === productData.id);
      if (index > -1) {
        products[index] = productData;
      }
    } else {
      // 新增产品
      productData.id = storage.generateId();
      productData.stock = 0;
      products.push(productData);
    }

    storage.saveData('products', products);

    wx.showToast({
      title: '保存成功',
      icon: 'success',
      success: () => {
        setTimeout(() => {
          wx.navigateBack();
        }, 1500);
      }
    });
  },

  // 删除产品
  deleteProduct() {
    wx.showModal({
      title: '确认删除',
      content: '确定要删除该产品吗？',
      success: (res) => {
        if (res.confirm) {
          const products = storage.getData('products');
          const newProducts = products.filter(p => p.id !== this.data.formData.id);
          storage.saveData('products', newProducts);
          
          wx.showToast({
            title: '删除成功',
            icon: 'success',
            success: () => {
              setTimeout(() => {
                wx.navigateBack();
              }, 1500);
            }
          });
        }
      }
    });
  }
});