import { useState, useEffect } from 'react';
import { useGetMyUserQuery } from '../entities/MyUser';

export const IsDesktop = (): boolean => {
    const query = '(min-width: 768px)';
    const [matches, setMatches] = useState(false);

    useEffect(() => {
        const media = window.matchMedia(query);
        setMatches(media.matches);

        const listener = (e: MediaQueryListEvent) => setMatches(e.matches);
        media.addEventListener('change', listener);
        return () => media.removeEventListener('change', listener);
    }, [query]);

    return matches;
};

export const GetHeaderHeight = (hideHeader?: boolean) => {
    if (hideHeader)
        return 0;

    const { data: userData, isLoading } = useGetMyUserQuery();
    const isAuthenticated = !isLoading && userData?.data != undefined;

    // Базовая высота хедера (на мобилке 40px, на ПК 48px)
    const baseHeight = window.innerWidth >= 768 ? 48 : 40;

    // Панель статистики показываем только авторизованным
    const statsHeight = isAuthenticated ? (window.innerWidth >= 768 ? 32 : 28) : 0;

    return baseHeight + statsHeight;
};

export const GetFooterHeight = (hideFooter?: boolean) => {
    return window.innerWidth >= 768
        ? 0
        : hideFooter ? 0 : (window.innerWidth >= 768 ? 64 : 56);
};