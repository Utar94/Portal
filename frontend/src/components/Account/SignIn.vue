<template>
  <b-container>
    <h1 v-t="'user.signIn.title'" />
    <b-alert dismissible variant="warning" v-model="accountIsDisabled">
      <strong v-t="'user.signIn.failed'" />
      {{ $t('user.signIn.accountIsDisabled') }}
    </b-alert>
    <b-alert dismissible variant="warning" v-model="invalidCredentials">
      <strong v-t="'user.signIn.failed'" />
      {{ $t('user.signIn.invalidCredentials') }}
    </b-alert>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <username-field required v-model="username" />
        <password-field ref="password" required v-model="password" />
        <b-form-group>
          <b-form-checkbox v-model="remember">{{ $t('user.signIn.remember') }}</b-form-checkbox>
        </b-form-group>
        <icon-submit :disabled="loading" icon="sign-in-alt" :loading="loading" text="user.signIn.submit" variant="primary" />
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import PasswordField from '@/components/User/PasswordField.vue'
import UsernameField from '@/components/User/UsernameField.vue'
import { signIn } from '@/api/account'

export default {
  name: 'SignIn',
  components: {
    PasswordField,
    UsernameField
  },
  props: {
    returnUrl: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      accountIsDisabled: false,
      invalidCredentials: false,
      loading: false,
      password: null,
      remember: false,
      username: null
    }
  },
  computed: {
    payload() {
      return {
        username: this.username,
        password: this.password,
        remember: this.remember
      }
    }
  },
  methods: {
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.accountIsDisabled = false
        this.invalidCredentials = false
        try {
          if (await this.$refs.form.validate()) {
            await signIn(this.payload)
            this.password = null
            this.$refs.form.reset()
            window.location.replace(this.returnUrl || '/user/profile')
          }
        } catch (e) {
          this.password = null
          this.$refs.password.focus()
          const { data, status } = e
          if (status === 400) {
            if (data?.code === 'AccountIsDisabled') {
              this.accountIsDisabled = true
            } else if (data?.code === 'InvalidCredentials') {
              this.invalidCredentials = true
            }
          } else {
            this.handleError(e)
          }
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
