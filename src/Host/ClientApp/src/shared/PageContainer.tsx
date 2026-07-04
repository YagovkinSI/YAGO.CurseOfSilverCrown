import React from 'react';
import { useGetMyUserQuery } from '../entities/MyUser';

interface PageContainerProps {
    children: React.ReactNode;
    className?: string;
    backgroundImage?: string;
    darkenBackground?: boolean;
    hideHeader?: boolean;
    hideFooter?: boolean;
}

const PageContainer: React.FC<PageContainerProps> = ({ 
    children, 
    className = '', 
    backgroundImage = 'space',
    darkenBackground = false,
    hideHeader = false,
    hideFooter = false,
}) => {
    // Получаем данные о пользователе
    const { data: userData, isLoading } = useGetMyUserQuery();
    const isAuthenticated = !isLoading && userData?.data != undefined;

    // Вычисляем высоту хедера (с учётом панели статистики)
    const getHeaderHeight = () => {
        if (hideHeader) return 0;
        
        // Базовая высота хедера (на мобилке 40px, на ПК 48px)
        const baseHeight = window.innerWidth >= 768 ? 48 : 40;
        
        // Панель статистики показываем только авторизованным
        const statsHeight = isAuthenticated ? (window.innerWidth >= 768 ? 32 : 28) : 0;
        
        return baseHeight + statsHeight;
    };

    const getFooterHeight = () => {
        return hideFooter ? 0 : (window.innerWidth >= 768 ? 64 : 56);
    };

    const [headerHeight, setHeaderHeight] = React.useState(getHeaderHeight);
    const [footerHeight, setFooterHeight] = React.useState(getFooterHeight);

    // Обновляем при изменении размера окна или статуса авторизации
    React.useEffect(() => {
        const updateHeights = () => {
            setHeaderHeight(getHeaderHeight());
            setFooterHeight(getFooterHeight());
        };

        updateHeights();
        window.addEventListener('resize', updateHeights);
        return () => window.removeEventListener('resize', updateHeights);
    }, [hideHeader, hideFooter, isAuthenticated]);

    return (
        <div 
            className={`
                fixed inset-0
                bg-cover bg-center bg-fixed
                ${className}
            `}
            style={{ 
                backgroundImage: `url('/images/pictures/${backgroundImage}.jpg')` 
            }}
        >
            {darkenBackground && (
                <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]" />
            )}

            <div 
                className="absolute inset-0 overflow-y-auto overscroll-contain"
                style={{
                    top: `${headerHeight}px`,
                    bottom: `${footerHeight}px`,
                    left: '16px',
                    right: '16px',
                }}
            >
                <div className="flex flex-col items-center justify-center w-full min-h-full">
                    {children}
                </div>
            </div>
        </div>
    );
};

export default PageContainer;