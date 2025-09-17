// app.js
const storage = require('./utils/storage.js');

App({
  onLaunch() {
    // 初始化本地存储
    storage.initStorage();
  },
  globalData: {
    userInfo: null
  }
})
