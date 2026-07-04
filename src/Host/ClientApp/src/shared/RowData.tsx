import type { LucideIcon } from "lucide-react";

export interface RowDataProps {
    color: string,
    icon: LucideIcon,
    label: string,
    value: string,
    url?: string | undefined
}