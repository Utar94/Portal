import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faAt,
  faBan,
  faCheck,
  faChessRook,
  faClipboard,
  faCog,
  faDice,
  faEnvelope,
  faExclamationTriangle,
  faExternalLinkAlt,
  faEye,
  faHome,
  faIdCard,
  faInfoCircle,
  faKey,
  faLock,
  faPaperPlane,
  faPlus,
  faSave,
  faSignInAlt,
  faSignOutAlt,
  faStar,
  faSyncAlt,
  faTimes,
  faTrashAlt,
  faUnlock,
  faUser,
  faUserAltSlash,
  faUserClock,
  faUsers,
  faVial
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(
  faAt,
  faBan,
  faCheck,
  faChessRook,
  faClipboard,
  faCog,
  faDice,
  faEnvelope,
  faExclamationTriangle,
  faExternalLinkAlt,
  faEye,
  faHome,
  faIdCard,
  faInfoCircle,
  faKey,
  faLock,
  faPaperPlane,
  faPlus,
  faSave,
  faSignInAlt,
  faSignOutAlt,
  faStar,
  faSyncAlt,
  faTimes,
  faTrashAlt,
  faUnlock,
  faUser,
  faUserClock,
  faUserAltSlash,
  faUsers,
  faVial
)

Vue.component('font-awesome-icon', FontAwesomeIcon)
