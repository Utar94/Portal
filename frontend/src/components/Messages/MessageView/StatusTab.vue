<template>
  <b-tab :title="$t('messages.status.label')">
    <status-detail :model="message" />
    <p>
      <template v-if="realm">
        {{ $t('messages.realmFormat') }}
        <b-link :href="`/realms/${realm.alias}`" target="_blank">{{ `${realm.name} (${realm.alias})` }} <font-awesome-icon icon="external-link-alt" /></b-link>.
      </template>
      <template v-else-if="message.realmId">{{ $t('messages.realmFormat') }} <strong v-text="`${message.realmName} (${message.realmAlias})`" />.</template>
      <template v-else>{{ $t('messages.noRealm.part1') }} <strong v-t="'messages.noRealm.part2'" />.</template>
      <br />
      <template v-if="sender">
        {{ $t('messages.sender.format1') }}
        <b-link v-if="sender.displayName" :href="`/senders/${sender.id}`" target="_blank">
          {{ sender.displayName }} &lt;{{ sender.emailAddress }}&gt; <font-awesome-icon icon="external-link-alt" />
        </b-link>
        <b-link v-else :href="`/senders/${sender.id}`" target="_blank">{{ sender.emailAddress }} <font-awesome-icon icon="external-link-alt" /></b-link>
        {{ ' ' }}
        <b-badge v-if="message.senderIsDefault" variant="info" v-t="'messages.sender.default'" />
        {{ $t('messages.sender.format2') }}
        <strong>{{ $t(`senders.provider.options.${message.senderProvider}`) }}</strong>
        .
      </template>
      <template v-else>
        {{ $t('messages.sender.format1') }}
        <strong v-if="message.senderDisplayName"> {{ message.senderDisplayName }} &lt;{{ message.senderAddress }}&gt;</strong>
        <strong v-else v-text="message.senderAddress" />
        {{ ' ' }}
        <b-badge v-if="message.senderIsDefault" variant="info" v-t="'messages.sender.default'" />
        {{ $t('messages.sender.format2') }}
        <strong>{{ $t(`senders.provider.options.${message.senderProvider}`) }}</strong>
        .
      </template>
      <br />
      {{ $t('messages.status.format') }} <status-badge :message="message" />
      <template v-if="message.isDemo">
        <br />
        <b-badge variant="info">{{ $t('messages.demo.label') }}</b-badge> <i class="text-info" v-t="'messages.demo.text'" />
      </template>
    </p>
    <div v-if="status === 'Sent'">
      <h3 v-t="'messages.successData'" />
      <json-viewer :value="result" :expand-depth="5" copyable boxed />
    </div>
    <div v-if="status === 'Failed'">
      <h3 v-t="'messages.failureData'" />
      <json-viewer :value="errors" :expand-depth="5" copyable boxed />
    </div>
    <p v-else-if="status === 'Unsent'" v-t="'messages.noResult'" />
  </b-tab>
</template>

<script>
import StatusBadge from '../StatusBadge.vue'
import { getRealm } from '@/api/realms'
import { getSender } from '@/api/senders'

export default {
  name: 'StatusTab',
  components: {
    StatusBadge
  },
  props: {
    message: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      realm: null,
      sender: null
    }
  },
  computed: {
    errors() {
      return this.message.errors.map(({ code, description, data }) => ({
        code,
        description,
        data: Object.fromEntries(data.map(({ key, value }) => [key, value]))
      }))
    },
    result() {
      return this.message.result ? Object.fromEntries(this.message.result.map(({ key, value }) => [key, value])) : null
    },
    status() {
      return this.message.succeeded ? 'Sent' : this.message.hasErrors ? 'Failed' : 'Unsent'
    }
  },
  async created() {
    try {
      if (this.message.realmId) {
        const { data } = await getRealm(this.message.realmId)
        this.realm = data
      }
    } catch (e) {
      if (e.status !== 404) {
        this.handleError(e)
      }
    }
    try {
      const { data } = await getSender(this.message.senderId)
      this.sender = data
    } catch (e) {
      if (e.status !== 404) {
        this.handleError(e)
      }
    }
  }
}
</script>
