import { get, patch } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function getSessions(params) {
  return await get('/api/sessions' + getQueryString(params))
}

export async function signOut(id) {
  return await patch(`/api/sessions/${id}/sign/out`)
}

export async function signOutAll(userId) {
  return await patch(`/api/users/${userId}/sessions/sign/out`)
}
