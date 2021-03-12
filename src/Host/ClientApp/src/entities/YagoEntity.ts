export default interface YagoEntity {
    id: number,
    name: string,
    type: 'Unknown' | 'User' | 'GameSession'
}