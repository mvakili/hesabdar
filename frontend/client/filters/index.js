module.exports = {
  'balance': function (value) {
    if (value < 0) {
      return 'بدهکار'
    } else if (value > 0) {
      return 'بستانکار'
    } else {
      return ''
    }
  },
  'abs': function (value) {
    return Math.abs(value)
  }
}
