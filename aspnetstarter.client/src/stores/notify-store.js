
import {defineStore} from "pinia"
import {CircleAlertIcon, CircleCheckIcon, CircleXIcon, InfoIcon} from "lucide-vue-next";

/**
 * @enum {string}
 */
export const NotificationType = {
  Info: "info",
  Success: "success",
  Warning: "warning",
  Error: "error",
}

/**
 * @typedef {Object} Notification
 * @property {number} id
 * @property {string} message
 * @property {string} type - NotificationType
 * @property {boolean} soft
 * @property {number} notifyTime
 */

/**
 * @type {Record<string, any>}
 */
export const NotificationTypeIcons = {
  [NotificationType.Info]: InfoIcon,
  [NotificationType.Success]: CircleCheckIcon,
  [NotificationType.Warning]: CircleAlertIcon,
  [NotificationType.Error]: CircleXIcon,
};

/**
 * @typedef {Object} State
 * @property {Notification[]} notifications
 * @property {Notification[]} notificationsArchive
 * @property {number} nextId
 */

export const useNotifyStore = defineStore('notify', {
  state: () => {
    return {
      notifications: [],
      notificationsArchive: [],
      nextId: 0,
    }
  },
  actions: {
    /**
     * Add a notification
     * @param {unknown} messageOrError - The message or error to notify
     * @param {string} type - NotificationType
     * @param {boolean} [soft=false] - Whether it's a soft notification
     */
    notify(messageOrError, type, soft) {
      let message = "";
      if (messageOrError instanceof Error) {message = messageOrError.message;}
      if (typeof messageOrError === "string") {message = messageOrError;}
      const notification = {
        message,
        type,
        notifyTime: Date.now(),
        id: this.nextId++,
        soft: soft ?? false
      };
      this.notifications.push(notification);
      setTimeout(this.removeNotification.bind(this), 5000, notification);
    },
    /**
     * Remove a notification
     * @param {Notification} notification
     */
    removeNotification(notification) {
      this.notifications = this.notifications.filter(n => n.id !== notification.id);
    },
  }
});

