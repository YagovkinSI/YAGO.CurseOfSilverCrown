import React from 'react';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import type { SerializedError } from '@reduxjs/toolkit';
import { BackgroundPage } from './BackgroundPage';
import { LoaderPage } from './LoaderPage';

interface PageProps {
    children: React.ReactNode;
    isLoading: boolean,
    error: FetchBaseQueryError | SerializedError | string | undefined,
    backgroundImage?: string;
    darkenBackground?: boolean;
}

const Page: React.FC<PageProps> = ({
    children,
    isLoading,
    error,
    backgroundImage,
    darkenBackground = false,
}) => {

    return (
        <LoaderPage isLoading={isLoading} error={error}>
            <BackgroundPage
                backgroundImage={backgroundImage}
                darkenBackground={darkenBackground}
            >
                {children}
            </BackgroundPage>
        </LoaderPage>
    );
};

export default Page;