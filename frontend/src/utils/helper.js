import { format, isToday, isYesterday } from "date-fns";

export const formatTime = (time) => {
  const date = new Date(time);

  if (isToday(date)) {
    return format(date, "HH:mm");
  } else if (isYesterday(date)) {
    return "Yesterday";
  } else {
    return format(date, "yyyy-MM-dd");
  }
};
