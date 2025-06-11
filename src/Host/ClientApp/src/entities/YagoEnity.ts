type YagoEntityType = 'Unknown' | 'Province';

export default interface YagoEnity {
    id: number,
    type: YagoEntityType,
    name: string
}