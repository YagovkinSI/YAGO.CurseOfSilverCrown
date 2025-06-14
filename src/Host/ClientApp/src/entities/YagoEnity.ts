type YagoEntityType = 'Unknown' | 'Province' | 'Faction';

export default interface YagoEnity {
    id: number,
    type: YagoEntityType,
    name: string
}