import { defineStore } from "pinia"
import {CircleAlertIcon, CircleCheckIcon, CircleXIcon, InfoIcon} from "lucide-vue-next";
import type {FunctionalComponent} from "vue";

/*
This store manages User and Account state including the ActiveAccount
It is used in the Account administration page and the header due to it's account switching features.
*/
export interface Notification{
  id: number;
  message: string;
  type: NotificationType;
  soft: boolean;
  notifyTime: number;
}

export enum NotificationType {
  Info = "info",
  Success = "success",
  Warning = "warning",
  Error = "error",
}

export const NotificationTypeIcons: Record<NotificationType, FunctionalComponent> = {
  [NotificationType.Info]: InfoIcon,
  [NotificationType.Success]: CircleCheckIcon,
  [NotificationType.Warning]: CircleAlertIcon,
  [NotificationType.Error]: CircleXIcon,
};

interface State {
  notifications: Notification[],
  notificationsArchive: Notification[],
  nextId: number,
}

export const useNotifyStore = defineStore('notify', {
  state: (): State => {
    return {
      notifications: [],
      notificationsArchive: [],
      nextId: 0,
    }
  },
  actions: {
    notify(messageOrError: unknown, type:NotificationType, soft?: boolean){
      let message: string = "";
      if (messageOrError instanceof Error) message = messageOrError.message;
      if (typeof messageOrError === "string") message = messageOrError;
      const notification: Notification = {message, type, notifyTime: Date.now(), id: this.nextId++, soft: soft ?? false};
      this.notifications.push(notification);
      setTimeout(this.removeNotification.bind(this), 5000, notification);
    },
    removeNotification(notification: Notification){
      this.notifications = this.notifications.filter(n => n.id != notification.id);
    },
  }
});
