import type { SerializedError } from "@reduxjs/toolkit";
import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import LoadingCard from "./LoadingCard";
import ErrorCard from "./ErrorCard";
import { BaseContainer } from "./BaseContainer";

interface LoaderPageProps {
    children: React.ReactNode;
    isLoading: boolean;
    error?: FetchBaseQueryError | SerializedError | string;
}

export const LoaderPage: React.FC<LoaderPageProps> = ({
    children,
    isLoading,
    error,
}) => {

    if (isLoading) {
        return (
            <BaseContainer className="p-2 overflow-y-auto scrollbar-hide">
                <LoadingCard />
            </BaseContainer>
        );
    }

    if (error) {
        return (
            <BaseContainer className="p-2 overflow-y-auto scrollbar-hide">
                <ErrorCard error={error} />
            </BaseContainer>
        );
    }

    return children;
};