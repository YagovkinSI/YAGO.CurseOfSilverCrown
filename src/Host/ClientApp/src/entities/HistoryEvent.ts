export const HistoryEventLevelList = {
  Unknown: 0,
  World: 1,
} as const;

export type HistoryEventLevel = typeof HistoryEventLevelList[keyof typeof HistoryEventLevelList];

export default interface HistoryEvent {
  id: number,
  level: HistoryEventLevel;
  entityId: number,
  dateTime: string,
  shortText: string
}