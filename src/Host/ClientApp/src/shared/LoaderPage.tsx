import type { SerializedError } from "@reduxjs/toolkit";
import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import LoadingCard from "./LoadingCard";
import ErrorCard from "./ErrorCard";
import { FlexContainer } from "./FlexContainer";

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
            <FlexContainer className="p-2 overflow-y-auto scrollbar-hide">
                <LoadingCard />
            </FlexContainer>
        );
    }

    if (error) {
        return (
            <FlexContainer className="p-2 overflow-y-auto scrollbar-hide">
                <ErrorCard error={error} />
            </FlexContainer>
        );
    }

    return children;
};