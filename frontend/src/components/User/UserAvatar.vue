<template>
  <span>
    <b-avatar v-if="user.picture || !user.email" :alt="alt" :size="size" :src="user.picture" variant="dark" />
    <v-gravatar v-else class="rounded-circle" :email="user.email" :size="size" />
  </span>
</template>

<script>
export default {
  name: 'UserAvatar',
  props: {
    size: {
      type: Number,
      default: 32
    },
    user: {
      type: Object,
      required: true
    }
  },
  computed: {
    alt() {
      return `${this.user.fullName ?? this.user.username}'s avatar`
    }
  }
}
</script>
