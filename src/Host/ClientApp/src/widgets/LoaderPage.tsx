import type { SerializedError } from "@reduxjs/toolkit";
import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import ErrorCard from "../shared/ui/ErrorCard";
import { FlexContainer } from "../shared/ui/FlexContainer";

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
            <FlexContainer className="h-full p-2 l-2">
                <div className="w-12 h-12 border-4 border-bright/20 border-t-bright rounded-full animate-spin" />
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