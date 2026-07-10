import React from 'react';
import { GetFooterHeight, GetHeaderHeight, IsDesktop } from '../features/MediaHelper';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import type { SerializedError } from '@reduxjs/toolkit';
import LoadingCard from './LoadingCard';
import ErrorCard from './ErrorCard';

interface PageContainerProps {
    children: React.ReactNode;
    isLoading: boolean,
    error: FetchBaseQueryError | SerializedError | string | undefined,
    justifyContent?: string;
    backgroundImage?: string;
    darkenBackground?: boolean;
    hideHeader?: boolean;
    hideFooter?: boolean;
    className?: string;
}

const PageContainer: React.FC<PageContainerProps> = ({
    children,
    isLoading,
    error,
    justifyContent = 'center',
    backgroundImage,
    darkenBackground = false,
    hideHeader = false,
    hideFooter = false,
    className = '',
}) => {
    const isDesktop = IsDesktop();
    const headerHeight = GetHeaderHeight(hideHeader);
    const footerHeight = GetFooterHeight(hideFooter);

    const renderContent = () => (
        <div
            className="absolute inset-0 overflow-y-auto overscroll-contain"
            style={{
                top: `${headerHeight}px`, bottom: `${footerHeight}px`, left: '16px', right: '16px',
            }}
        >
            <div
                className="flex flex-col items-center justify-center w-full min-h-full py-2"
                style={{ justifyContent: `${justifyContent}` }}
            >
                {isLoading && <LoadingCard />}
                {!isLoading && error && <ErrorCard error={error!} />}
                {!isLoading && !error && children}
            </div>
        </div>
    )

    return (
        <div
            className={`
                fixed inset-0 bg-cover bg-center bg-fixed
                ${isDesktop ? 'ml-64' : ''} ${className}
            `}
            style={{
                backgroundImage: `url('/images/pictures/${backgroundImage}.jpg')`,
                backgroundPosition: isDesktop ? 'calc(50% + 128px) center' : 'center'
            }}
        >
            {darkenBackground && (
                <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]" />
            )}
            {renderContent()}
        </div>
    );
};

export default PageContainer;