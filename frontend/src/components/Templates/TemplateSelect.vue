<template>
  <form-select :id="id" :label="label" :options="options" :placeholder="placeholder" :required="required" :value="value" @input="$emit('input', $event)" />
</template>

<script>
import { getTemplates } from '@/api/templates'

export default {
  name: 'TemplateSelect',
  props: {
    id: {
      type: String,
      default: 'template'
    },
    label: {
      type: String,
      default: 'templates.select.label'
    },
    placeholder: {
      type: String,
      default: 'templates.select.placeholder'
    },
    realm: {},
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      templates: []
    }
  },
  computed: {
    options() {
      return this.templates.map(({ id, displayName, key }) => ({
        text: displayName ? `${displayName} (${key})` : key,
        value: id
      }))
    }
  },
  methods: {
    async refresh(realm) {
      try {
        const { data } = await getTemplates({
          realm,
          sort: 'Key',
          desc: false
        })
        this.templates = data.items
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  watch: {
    realm: {
      immediate: true,
      handler(realm) {
        this.refresh(realm)
      }
    }
  }
}
</script>
