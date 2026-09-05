import type { CouncilPosition } from './council.types';

export const councilPositions: CouncilPosition[] = [
    {
        code: 'administrator',
        title: 'Администратор',
        description: 'Координатор станции. Отвечает за связь с Консорциумом и общее управление. Открывает доступ к найму других советников.',
        member: null,
    },
    {
        code: 'engineer',
        title: 'Инженер станции',
        description: 'Следит за реактором, водой, воздухом и энергией. Позволяет расширять станцию и модернизировать модули',
        member: null,
    },
    {
        code: 'financier',
        title: 'Финансист',
        description: 'Управляет бюджетом, налогами и отчётностью. Позволяет проводить реформы и заключать контракты.',
        member: null,
    },
    {
        code: 'social',
        title: 'Социальный советник',
        description: 'Отвечает за найм, удержание людей и внутренний климат. Обеспечивает рост населения и предотвращает конфликты.',
        member: null,
    },
];