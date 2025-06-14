export const YagoEntityTypeList = {
  Unknown: 0,
  Province: 1,
  Faction: 2
} as const;

export type YagoEntityType = typeof YagoEntityTypeList[keyof typeof YagoEntityTypeList];

export default interface YagoEnity {
    id: number,
    type: YagoEntityType,
    name: string
}