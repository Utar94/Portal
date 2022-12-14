import { extend } from 'vee-validate'

extend('username', {
  validate(value, { allowedCharacters }) {
    return typeof value === 'string' && [...value].every(c => allowedCharacters.includes(c))
  },
  params: ['allowedCharacters']
})
